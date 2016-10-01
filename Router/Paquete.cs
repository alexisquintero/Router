using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Router
{
    class Paquete
    {  
        public int prioridad { get; set; }  //Valor de sumar la prioridad de la cola por la prioridad del paquete
//        public int computadora { get; set; }    //Número de computadora que generó el paquete, asignado por el router ya que es
                                                //la posición en la List<Computadora>
        public Paquete(bool prioridadPaquete, Enumeradores.PrioridadCola prioridadCola)
        {            
            prioridad = prioridadPaquete ? 0 : (int)prioridadCola;
        }
    }
}
