using System;
using System.Collections.Generic;

namespace BTP.Models
{
    public class Data
    {
        public List<Maison> maisonList = new List<Maison>();
        public List<Finition> finiList = new List<Finition>();

        public Data()
        {
            this.maisonList = new List<Maison>();
            this.finiList = new List<Finition>();
        }
    }
}