using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculadora2.Clases
{
    internal class CPorcentaje
    {
        public double Porcentuar(double N1, double N2, string operador)
        {
            double resultado = 0;
            switch (operador)
            {
                case "+":
                case "-":
                    resultado = N1 * N2 / 100;
                    break;
                case "*":
                case "/":
                    resultado = N2 / 100;
                    break;
            }
            return resultado;
        }
    }
}

