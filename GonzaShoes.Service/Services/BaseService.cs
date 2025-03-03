using GonzaShoes.Service.Interfaces;

namespace GonzaShoes.Service.Services
{
    public abstract class BaseService : IBaseService
    {
        protected int userId;

        public virtual void SetCurrentUser(int userId)
        {
            this.userId = userId;
        }
    }
}
