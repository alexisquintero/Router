using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Priority_Queue;

namespace Router
{
    class Modem
    {
        private Queue<Paquete> colaFIFO = new Queue<Paquete>();
        private SimplePriorityQueue<Paquete> colaPrioridad = new SimplePriorityQueue<Paquete>();
        public double mu { get; set; }
        public int tamanioMaximoCola { get; set; }
        public Enumeradores.modo modo { get; set; }
        public void agregarACola(Paquete paquete)
        {
            switch (modo)
            {
                case Enumeradores.modo.FIFO:
                    if (colaLlena())
                    {
                        if(paquete.prioridad == 0)
                        {
                            Simulador.cantidadDeServiciosNegadosPrioridadAlta++;
                        }
                        else
                        {
                            Simulador.cantidadDeServiciosNegadosPrioridadNormal++;
                        }
                    }
                    else
                    {
                        colaFIFO.Enqueue(paquete);
                    }
                    break;
                case Enumeradores.modo.Prioridades:                   
                    if (colaLlena())
                    {
                        if (paquete.prioridad == 0)
                        {
                            Simulador.cantidadDeServiciosNegadosPrioridadAlta++;
                        }
                        else
                        {
                            Simulador.cantidadDeServiciosNegadosPrioridadNormal++;
                        }                       
                    }
                    else
                    {
                        colaPrioridad.Enqueue(paquete, paquete.prioridad);
                    }
                    break;
                default:
                    break;
            }            
        }
        public double generarTiempoServicio()
        {
            return Generador.generaServicio(mu);
        }
        public bool colaLlena()
        {
            switch (modo)
            {
                case Enumeradores.modo.FIFO:
                    return (colaFIFO.Count >= tamanioMaximoCola);
                case Enumeradores.modo.Prioridades:
                    return (colaPrioridad.Count >= tamanioMaximoCola);
                default:
                    return true;
            }          
        }
        public bool colaVacia()
        {
            switch (modo)
            {
                case Enumeradores.modo.FIFO:
                    return (colaFIFO.Count == 0);
                case Enumeradores.modo.Prioridades:
                    return (colaPrioridad.Count == 0);
                default:
                    return true;
            }
        }
        public void quitarPaquete()
        {
            switch (modo)
            {
                case Enumeradores.modo.FIFO:
                    colaFIFO.Dequeue();
                    break;
                case Enumeradores.modo.Prioridades:
                    colaPrioridad.Dequeue();
                    break;
                default:
                    break;
            }           
        }
        public int cantidadDePaquetesEnCola()
        {
            switch (modo)
            {
                case Enumeradores.modo.FIFO:
                    return colaFIFO.Count;
                case Enumeradores.modo.Prioridades:
                    return colaPrioridad.Count;
                default:
                    return 0;
            }          
        }
    }
}
