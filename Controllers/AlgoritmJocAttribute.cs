namespace Backend_Obstruction.Controllers
{
    [AttributeUsage(AttributeTargets.Method)]
    public class AlgoritmJocAttribute : Attribute
    {
        public string DenumireAlgoritm { get; private set; }
        public AlgoritmJocAttribute(string DenumireAlgoritm)
        {
            this.DenumireAlgoritm = DenumireAlgoritm;
        }
    }
}
