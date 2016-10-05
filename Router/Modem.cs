using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Router
{
    class Modem
    {
        private Queue<Paquete> colaFIFO = new Queue<Paquete>(); 
        private List<Paquete> colaPrioridad = new List<Paquete>();
        public double mu { get; set; }
        public int tamanioMaximoCola { get; set; }
        public Enumeradores.modo modo { get; set; }
        private bool modoNormalColaPrioridad = true;    
                                                        
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
                    if (modoNormalColaPrioridad)
                    {
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
                            colaPrioridad.Add(paquete);                           
                            colaPrioridad = colaPrioridad.OrderBy(p => p).ToList();
                        } 
                    }
                    
                    else
                    {
                        if (colaLlena())                                                        //Si la cola está llena
                        {
                            if(paquete.prioridad == 0)                                          //y el paquete tiene prioridad alta
                            {
                                if (colaPrioridad.Last().prioridad != 0)                        //y el último paquete de la cola no tiene prioridad alta
                                {
                                    colaPrioridad.Remove(colaPrioridad.Last());                 //se elimina el último paquete de la cola y se agrega el paquete 
                                    colaPrioridad.Add(paquete);                                 //con prioridad alta
                                    colaPrioridad = colaPrioridad.OrderBy(p => p).ToList();
                                    Simulador.cantidadDeServiciosNegadosPrioridadNormal++;
                                }
                                else
                                {
                                    Simulador.cantidadDeServiciosNegadosPrioridadAlta++;
                                }
                            }
                            else                                                                //Si no es un paquete con prioridad alta se le niega servicios
                            {
                                Simulador.cantidadDeServiciosNegadosPrioridadNormal++;
                            }
                        }
                        else
                        {
                            colaPrioridad.Add(paquete);
                            colaPrioridad = colaPrioridad.OrderBy(p => p).ToList();
                        }
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
                    colaPrioridad.Remove(colaPrioridad.Last());
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
