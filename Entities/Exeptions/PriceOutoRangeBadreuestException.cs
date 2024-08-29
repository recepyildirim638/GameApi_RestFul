namespace Entities.Exeptions
{
    public class PriceOutoRangeBadreuestException : BadRequestException
    {
        public PriceOutoRangeBadreuestException() : base("Maximum pride should be less than 1000 and greater than 10.")
        {

        }
    }
}
