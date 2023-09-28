namespace DentalClinic.API.Data
{
    public class Dentist
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Specialization { get; set; }

        public virtual IList<Appointment> Appointments { get; set; }
        
    }

   


}
