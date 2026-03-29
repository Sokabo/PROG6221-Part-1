namespace MegaMind
{
    
    // Holds identity information and can be extended in future
    public class User
    {
       
        //The user's display name, entered at startup.
        // Defaults to "Sir/Madam" if no name is provided.
        
        public string Name { get; set; } = "Sir/Madam";
    }
}