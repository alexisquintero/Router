using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Router
{
    class Simulador
    {
        //Medidas de rendimiento
        public int cantidadDeServiciosNegados { get; set; }
        public int cantidadDeServicios { get; set; }    //Solo los no negados

        //Variables de estado
        public double reloj { get; set; }
        public double tiempoFinSimulacion { get; set; }
        public double[] listaDeEventos { get; set; }    //Tiempo prox. arribo y prox. partida; Arribo = 0, Partida = 1
        public int nroComputadoraProximoEvento { get; set; }    //Nro. de la computadora que se va a utilizar en el prox. evento
        public Enumeradores.EstadoSistema estado { get; set; }  //0 = desocupado       

        //Otras variables

        private Enumeradores.ProximoEvento proximoEvento;
        private int nroDeComputadoras;
        private List<Computadora> computadoras;
        private Generador generador = new Generador();
        private Modem modem = new Modem();
        private double lambda;

        public void Simulacion()
        {
            Inicializacion();
            while (tiempoFinSimulacion > reloj)
            {
                Tiempos();
                switch (proximoEvento)
                {
                    case Enumeradores.ProximoEvento.Arribo:
                        Arribo();
                        break;
                    case Enumeradores.ProximoEvento.Partida:
                        Partida();
                        break;
                    default:
                        break;
                }
            }
            Reporte();
        }
        private void Inicializacion()
        {
            tiempoFinSimulacion = 10000;
            nroDeComputadoras = 4;
            modem.mu = 0.5;
            lambda = 0.4;
            modem.tamanioMaximoCola = 64;   
            modem.modo = Enumeradores.modo.FIFO;
            Paquete.valorPrioridadAlta = 5;

            reloj = 0;
            proximoEvento = Enumeradores.ProximoEvento.Arribo;
            estado = Enumeradores.EstadoSistema.Desocupado;
            modem.cantidadDePaquetesEnCola = 0;
            cantidadDeServicios = 0;
            cantidadDeServiciosNegados = 0;
            listaDeEventos[(int)Enumeradores.ProximoEvento.Partida] = tiempoFinSimulacion * 2;  //tiempo prox. partida = infinito

            inicializarComputadoras();
            generarProximoArribo();    
        }
        private void Tiempos()
        {

        }
        private void Arribo()
        {
            generarProximoArribo();
            if(estado == Enumeradores.EstadoSistema.Desocupado)
            {
                generarProximaPartida();
                cantidadDeServicios += 1;
                estado = Enumeradores.EstadoSistema.Ocupado;
            }
            else
            {
                if (modem.colaLlena())
                {
                    cantidadDeServiciosNegados += 1;
                }
                else
                {
                    modem.agregarACola(computadoras.ElementAt(nroComputadoraProximoEvento).generaPaquete());
                    modem.cantidadDePaquetesEnCola += 1;
                }
            }
        }
        private void Partida()
        {
            if (!modem.colaVacia())
            {
                generarProximaPartida();
                modem.cantidadDePaquetesEnCola -= 1;
                modem.quitarPaquete();
                cantidadDeServicios += 1;
            }
            else
            {
                estado = Enumeradores.EstadoSistema.Desocupado;
                listaDeEventos[(int)Enumeradores.ProximoEvento.Partida] = tiempoFinSimulacion * 2;
            }
        }
        private void Reporte()
        {
            Console.WriteLine("Cantidad de servicios: {0}", cantidadDeServicios);
            Console.WriteLine("Cantidad de servicios negados: {0}", cantidadDeServiciosNegados);
            Console.WriteLine("Proporción de servicios negados: {0}", cantidadDeServiciosNegados / cantidadDeServicios);
        }
        private void inicializarComputadoras()
        {
            //inicializa todas las computadoras iguales
            
            double probabilidadPaqueteAlta = 0.05;

            for (int i = 0; i < nroDeComputadoras; i++)
            {
                Computadora c = new Computadora(lambda, Enumeradores.PrioridadCola.Media, probabilidadPaqueteAlta, generador);
                computadoras.Add(c);
            }
        }    
        private void generarProximoArribo()
        {
            double min = tiempoFinSimulacion; //Guarda el tiempo más chico
            int nro = 0;    //Nro. de la computadora con el tiempo más chico
            for (int i = 0; i < nroDeComputadoras; i++)
            {
                if (computadoras.ElementAt(i).tiempoProximoPaquete < min)
                {
                    nro = i;
                }
            }
            listaDeEventos[(int)Enumeradores.ProximoEvento.Arribo] = reloj + min;
            nroComputadoraProximoEvento = nro;
        }
        private void generarProximaPartida()
        {
            listaDeEventos[(int)Enumeradores.ProximoEvento.Partida] = reloj + modem.generarTiempoServicio();
        }       
    }
}
