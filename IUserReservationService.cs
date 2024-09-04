namespace sports_management.Services
{
    public interface IUserReservationService
    {
        //更新用户的违约信息的方法签名
        void UpdateUserViolation(string userId);
    }
}
