namespace SmartHomeController
{
    class Program
    {
        public interface ISchakelbaar
        {
            bool IsAan { get; }
            void Aan();
            void Uit();
        }
        public interface IDimbaar
        {
            int Helderheid { get; } // 0 - 100
            void Dim(int percentage);
        }
        public interface ITemperatuurMeter
        {
            double MeetTemperatuur();
        }

        public interface IMelder
        {
            void Verstuur(string bericht);
        }

        public abstract class Apparaat
        {
            public string Naam { get; }
            public string Kamer { get; }
            protected Apparaat(string naam, string kamer)
            {
                Naam = naam;
                Kamer = kamer;
            }
            public abstract string GeefStatus();
            public override string ToString() => $"[{Kamer}] {Naam}: {GeefStatus()}";
        }


        public class Stopcontact : Apparaat, ISchakelbaar
        {
            private bool isAan {  get; set; }
            public bool IsAan { get { return isAan; } }

            public void Aan()
            {
                this.isAan = true;
            }

            public void Uit()
            {
                this.isAan = false;
            }

            public override string GeefStatus()
            {
                return $"Dit stopcontact is {IsAan}";
            }

            public Stopcontact(string naam, string kamer) : base (naam, kamer) {

            }

        }

        public class Thermostaat : Apparaat, ITemperatuurMeter
        {
            public Thermostaat(string name, string kamer) : base(name, kamer)
            {

            }
            
            public double MeetTemperatuur()
            {
                return 24;
            }

            public override string GeefStatus()
            {
                return $"De temperatuur is {MeetTemperatuur()} graden.";
            }
        }
        public class ConsoleMelder : IMelder
        {
            public void Verstuur(string bericht)
            {
                Console.WriteLine(bericht);
            }

            public ConsoleMelder()
            {

            }
        }

        public class EmailMelder : IMelder
        {
            public void Verstuur(string bericht)
            {
                throw new NotImplementedException();
            }

            public EmailMelder()
            {

            }
        }
        public class HuisController
        {
            private IMelder _melder = new ConsoleMelder(); // concrete afhankelijkheid!
            public void ControleerTemperatuur(ITemperatuurMeter meter)
            {
                if (meter.MeetTemperatuur() > 22)
                    _melder.Verstuur("Het is te warm!");
            }
        }

        static void Main(string[] args)
        {
            List<ISchakelbaar> schakelbareitems = new List<ISchakelbaar>();

            Stopcontact stopcontact1 = new Stopcontact("stopcontact1", "Woonkamer");
            schakelbareitems.Add(stopcontact1);

            Stopcontact stopcontact2 = new Stopcontact("Stopcontact2", "Slaapkamer");
            schakelbareitems.Add(stopcontact2);

            Thermostaat thermostaat = new Thermostaat("Naampje", "leuke kamer");

            HuisController huiscontroller = new HuisController();

            huiscontroller.ControleerTemperatuur(thermostaat);

        }
    }
}