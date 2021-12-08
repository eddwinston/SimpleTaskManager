namespace SimpleTaskManager.Domain
{
    public class ModelBase
    {
        public virtual int Id { get; private set; }

        internal ModelBase WithId(int id)
        {
            Id = id;

            return this;
        }
    }
}
