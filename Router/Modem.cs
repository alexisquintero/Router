using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Router
{
    class Modem
    {
        private Queue<Paquete> cola { get; set; }
        public double mu { get; set; }
        public int tamanioMaximoCola { get; set; }
        public int cantidadDePaquetesEnCola { get; set; }
        public Enumeradores.modo modo { get; set; }
        public void agregarACola(Paquete paquete)
        {
            if(modo == Enumeradores.modo.FIFO)
            {
                cola.Enqueue(paquete);
            }
            if(modo == Enumeradores.modo.Prioridades)
            {
                //Usar una list<>, agregar el paquete y hacer un sort
            }
            
        }
        public double generarTiempoServicio()
        {
            //TODO: programar, copiar del tp anterior
            return null;
        }
        public bool colaLlena()
        {
            return (cola.Count >= tamanioMaximoCola);
        }
        public bool colaVacia()
        {
            return (cola.Count == 0);
        }
        public void quitarPaquete()
        {
            cola.Dequeue();
        }
    }
}
