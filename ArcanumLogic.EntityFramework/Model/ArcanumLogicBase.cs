namespace ArcanumLogic.EntityFramework.Model
{
    public class ArcanumLogicBase
    {
        public ArcanumLogicBase() { }
        public ArcanumLogicBase(long id)
        {
            Id = id;
        }

        public long Id { get; set; }
    }
}