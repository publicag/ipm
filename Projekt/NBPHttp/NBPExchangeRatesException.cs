using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace NBPHttp
{
    public class NBPExchangeRatesException : Exception
    {
        public NBPExchangeRatesException()
        {
        }

        public NBPExchangeRatesException(string message) : base(message)
        {
        }

        public NBPExchangeRatesException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NBPExchangeRatesException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
