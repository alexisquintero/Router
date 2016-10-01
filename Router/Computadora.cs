using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Router
{
    class Computadora
    {
        public static int cantidadPaquetesPrioridadAlta = 0;
        private double lambda;
        public Enumeradores.PrioridadCola prioridad { get; set; }
        private double paqueteAlta; //Probabilidad de generar un paquete con prioridad alta
        public double tiempoProximoPaquete;
        public int numeroDePaquetes { get; set; }   //Se usa en debug

        public Computadora(double pLambda, Enumeradores.PrioridadCola pPrioridadCola, double probabilidadPaqueteAlta)
        {
            lambda = pLambda;
            prioridad = pPrioridadCola;
            paqueteAlta = probabilidadPaqueteAlta;
            generaTiempoProxPaquete();            
        }

        public void generaTiempoProxPaquete()
        {
            tiempoProximoPaquete = Generador.generaArribo(lambda);
            numeroDePaquetes++;
        }
        public Paquete generaPaquete()
        {           
            bool flagPrioridad = prioridadAlta() ? true : false;
            return new Paquete(flagPrioridad, prioridad);          
        }
        private bool prioridadAlta()
        {
            if( Generador.generaNumeroAleatorio() <= paqueteAlta)
            {
                cantidadPaquetesPrioridadAlta++;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
