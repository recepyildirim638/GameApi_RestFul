namespace Entities.Exeptions
{
    public abstract class NotFoundException : Exception
    {
        protected NotFoundException(string message) : base(message)
        {
            
        }
    }
}
