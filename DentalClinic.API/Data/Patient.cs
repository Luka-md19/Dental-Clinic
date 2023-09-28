namespace DentalClinic.API.Data
{

    public class Patient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public virtual IList<Appointment> Appointments { get; set; }
        public virtual IList<Invoice> Invoices { get; set; }

      
    }

   
}
