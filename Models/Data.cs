using System;
using System.Collections.Generic;

namespace BTP.Models
{
    public class Data
    {
        public List<Maison> maisonList = new List<Maison>();
        public List<Finition> finiList = new List<Finition>();
        public List<DemandeDevis> demandeList = new List<DemandeDevis>();
        public DemandeDevis infopaye = new DemandeDevis();
        public double montantTotalEnCours = new();
        public double montantDejaEffectue = new();
        public double montantTotalDesDevis = new();
        public List<int> listYear = new List<int>();
        public List<Chart> chartList = new List<Chart>();

        public Data()
        {
            this.maisonList = new List<Maison>();
            this.finiList = new List<Finition>();
            this.infopaye = new DemandeDevis();
            this.demandeList = new List<DemandeDevis>();
            this.montantTotalEnCours = 0;
            this.montantDejaEffectue = 0;
            this.montantTotalDesDevis = 0;
            this.listYear = new List<int>();
            this.chartList = new List<Chart>();
        }
    }
}