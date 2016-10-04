using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Router
{
    class Simulador
    {
        public Simulador()
        {
            modem = new Modem();
        }
        //Medidas de rendimiento
        //public static int cantidadDeServiciosNegados { get; set; }
        public static int cantidadDeServiciosNegadosPrioridadAlta { get; set; }
        public static int cantidadDeServiciosNegadosPrioridadNormal { get; set; }
        public int cantidadDeServicios { get; set; }    //Solo los no negados

        //Variables de estado
        public double reloj { get; set; }
        public double tiempoFinSimulacion { get; set; }
        private double[] listaDeEventos = new double[2];    //Tiempo prox. arribo y prox. partida; Arribo = 0, Partida = 1
        public int nroComputadoraProximoEvento { get; set; }    //Nro. de la computadora que se va a utilizar en el prox. evento
        public Enumeradores.EstadoSistema estado { get; set; }  //0 = desocupado       

        //Otras variables

        private Enumeradores.ProximoEvento proximoEvento;
        private int nroDeComputadoras;
        private List<Computadora> computadoras;
        private Modem modem;
        private double lambda;

        private bool flagDebug = false;

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
            tiempoFinSimulacion = 10000;       //en ms, 10 segundos
            nroDeComputadoras = 4;
            modem.mu = 812.74;                  //81274 packets por segundo con packets de 1500 bytes, 812.74 en 1 ms
            lambda = 812.74/8;                       
            modem.tamanioMaximoCola = 64;   
            modem.modo = Enumeradores.modo.Prioridades;

            reloj = 0;
            proximoEvento = Enumeradores.ProximoEvento.Arribo;
            estado = Enumeradores.EstadoSistema.Desocupado;
            cantidadDeServicios = 0;
            //cantidadDeServiciosNegados = 0;
            cantidadDeServiciosNegadosPrioridadAlta = 0;
            cantidadDeServiciosNegadosPrioridadNormal = 0;
            listaDeEventos[(int)Enumeradores.ProximoEvento.Partida] = tiempoFinSimulacion * 2;  //tiempo prox. partida = infinito
            computadoras = new List<Computadora>();

            inicializarComputadoras();
            generarProximoArribo();    
        }
        private void Tiempos()
        {
            if(listaDeEventos[(int)Enumeradores.ProximoEvento.Arribo] <= listaDeEventos[(int)Enumeradores.ProximoEvento.Partida])
            {
                proximoEvento = Enumeradores.ProximoEvento.Arribo;
                reloj += listaDeEventos[(int)Enumeradores.ProximoEvento.Arribo];
            }
            else
            {
                proximoEvento = Enumeradores.ProximoEvento.Partida;
                reloj += listaDeEventos[(int)Enumeradores.ProximoEvento.Partida];
            }
            if (flagDebug) { debug(); }      
        }
        private void Arribo()
        {
            generarProximoArribo();
            if(estado == Enumeradores.EstadoSistema.Desocupado)
            {
                computadoras.ElementAt(nroComputadoraProximoEvento).generaPaquete();    //se usa para controlar el porcentaje de paquetes creados con prioridad alta, nada más
                generarProximaPartida();
                cantidadDeServicios += 1;
                estado = Enumeradores.EstadoSistema.Ocupado;
            }
            else
            {
                modem.agregarACola(computadoras.ElementAt(nroComputadoraProximoEvento).generaPaquete());    //El modem se encarga de si la cola está llena o no
            }
        }
        private void Partida()
        {
            if (!modem.colaVacia())
            {
                generarProximaPartida();
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
            int serviciosNegados = cantidadDeServiciosNegadosPrioridadAlta + cantidadDeServiciosNegadosPrioridadNormal;
            int cantidadDePaquetes = cantidadDeServicios + serviciosNegados;
            double proporcionServiciosNegados = (((double)cantidadDeServiciosNegadosPrioridadAlta + (double)cantidadDeServiciosNegadosPrioridadNormal) / (double)cantidadDeServicios);
            double proporcionServiciosNegadosPrioridadAlta = (double)cantidadDeServiciosNegadosPrioridadAlta / (double)cantidadDeServicios;
            double proporcionServiciosNegadosPrioridadAltaDeServiciosNegados = (double)cantidadDeServiciosNegadosPrioridadAlta / ((double)cantidadDeServiciosNegadosPrioridadAlta + (double)cantidadDeServiciosNegadosPrioridadNormal);
            double proporcionServiciosNegadosPrioridadNormal = (double)cantidadDeServiciosNegadosPrioridadNormal / (double)cantidadDeServicios;
            double proporcionServiciosNegadosPrioridadNormalDeServiciosNegados = (double)cantidadDeServiciosNegadosPrioridadNormal / ((double)cantidadDeServiciosNegadosPrioridadAlta + (double)cantidadDeServiciosNegadosPrioridadNormal);

            Console.WriteLine("Cantidad de servicios: {0} \t Cantidad de paquetes creados: {1}", cantidadDeServicios, cantidadDePaquetes);
            Console.WriteLine("Cantidad de servicios negados: {0} \t Proporción: {1}", serviciosNegados, proporcionServiciosNegados.ToString("0.00000%"));
            Console.WriteLine("Cantidad de servicios negados con prioridad alta: {0} \t Proporcion: {1} \t proporcion de servicios negados: {2}", cantidadDeServiciosNegadosPrioridadAlta, proporcionServiciosNegadosPrioridadAlta.ToString("0.00000%"), proporcionServiciosNegadosPrioridadAltaDeServiciosNegados.ToString("0.00000%"));
            Console.WriteLine("Cantidad de servicios negados con prioridad normal: {0} \t Proporcion: {1} \t proporcion de servicios negados: {2}", cantidadDeServiciosNegadosPrioridadNormal, proporcionServiciosNegadosPrioridadNormal.ToString("0.00000%"), proporcionServiciosNegadosPrioridadNormalDeServiciosNegados.ToString("0.00000%"));
            Console.WriteLine("Número de paquetes con alta prioridad: {0} \t Proporcion: {1}", Computadora.cantidadPaquetesPrioridadAlta, ((double)Computadora.cantidadPaquetesPrioridadAlta / ((double)serviciosNegados + (double)cantidadDeServicios)).ToString("0.00000%"));
            Console.ReadLine();
        }
        private void inicializarComputadoras()
        {
            //inicializa todas las computadoras iguales
            
            double probabilidadPaqueteAlta = 0.05;

            for (int i = 0; i < nroDeComputadoras; i++)
            {
                Computadora c = new Computadora(lambda, Enumeradores.PrioridadCola.Media, probabilidadPaqueteAlta);
                computadoras.Add(c);
            }
        }    
        private void generarProximoArribo()
        {
            double min = tiempoFinSimulacion * 2; //Guarda el tiempo más chico
            int nro = 0;    //Nro. de la computadora con el tiempo más chico
            for (int i = 0; i < nroDeComputadoras; i++)
            {
                if (computadoras.ElementAt(i).tiempoProximoPaquete < min)
                {
                    nro = i;
                    min = computadoras.ElementAt(i).tiempoProximoPaquete;
                }
            }
            for (int i = 0; i < nroDeComputadoras; i++)         //Este loop le resta el tiempo más corto a los demás
            {                                                   
                if (!(i==nro))
                {
                    computadoras.ElementAt(i).tiempoProximoPaquete -= min;
                }
            }
            listaDeEventos[(int)Enumeradores.ProximoEvento.Arribo] = min;
            nroComputadoraProximoEvento = nro;
            computadoras.ElementAt(nro).generaTiempoProxPaquete();
        }
        private void generarProximaPartida()
        {
            listaDeEventos[(int)Enumeradores.ProximoEvento.Partida] = modem.generarTiempoServicio();
        }       
        private void debug()
        {
            //Muestra en consola datos
            Console.WriteLine("Reloj: {0}", reloj);
            Console.WriteLine("Lista de eventos \t Arribo: {0} \t Partida: {1}", listaDeEventos[(int)Enumeradores.ProximoEvento.Arribo], listaDeEventos[(int)Enumeradores.ProximoEvento.Partida]);
            for (int i = 0; i < nroDeComputadoras; i++)
            {
                Console.WriteLine("Nro. de computadora: {0} \t Tiempo prox. paquete {1} \t Nro. de paquetes: {2}", i, computadoras.ElementAt(i).tiempoProximoPaquete, computadoras.ElementAt(i).numeroDePaquetes);
            }                  
            //Console.WriteLine("Nro. de paquetes: {0} \t Con prioridad alta: {1} \t Negados: {2}", cantidadDeServicios + cantidadDeServiciosNegados, Computadora.cantidadPaquetesPrioridadAlta, cantidadDeServiciosNegados);
            Console.WriteLine("Tamaño de la cola: {0}", modem.cantidadDePaquetesEnCola());
            Console.WriteLine();
            Console.WriteLine();
            Console.ReadLine();
        }
    }
}
