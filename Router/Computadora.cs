using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Router
{
    class Computadora
    {
        public double lambda { get; set; }
        public Enumeradores.PrioridadCola prioridad { get; set; }
        public double paqueteAlta { get; set; } //Probabilidad de generar un paquete con prioridad alta
        public double tiempoProximoPaquete { get; set; }
        public Generador generador { get; set; }

        public Computadora(double pLambda, Enumeradores.PrioridadCola pPrioridadCola, double probabilidadPaqueteAlta, Generador pGenerador)
        {
            lambda = pLambda;
            prioridad = pPrioridadCola;
            paqueteAlta = probabilidadPaqueteAlta;
            tiempoProximoPaquete = generaTiempoProxPaquete();
            generador = pGenerador;
        }

        public double generaTiempoProxPaquete()
        {
            return generador.generaArribo(lambda);
        }
        public Paquete generaPaquete()
        {
            bool flagPrioridad = prioridadAlta() ? true : false;
            return new Paquete(flagPrioridad, prioridad);          
        }
        private bool prioridadAlta()
        {
            if( generador.generaNumeroAleatorio() <= paqueteAlta)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
