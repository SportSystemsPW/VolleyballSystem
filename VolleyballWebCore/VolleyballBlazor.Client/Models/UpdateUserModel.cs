using Volleyball.DTO.Teams;
using Volleyball.DTO.Users;
using VolleyballBlazor.Client.Shared.FormUtils;

namespace VolleyballBlazor.Client.Models
{
    public class UpdateUserModel : UpdateUserDto
    {
        [ExcludeFromForm]
        public List<PositionDto> PositionList { get; set; } = new List<PositionDto>();

        [ExcludeFromForm]
        public new bool Gender { get; set; }

        public UpdateUserModel() { }

        public UpdateUserModel(UserProfileDto userProfile)
        {
            FirstName = userProfile.FirstName;
            LastName = userProfile.LastName;
            BirthYear = userProfile.BirthYear ?? 0;
            City = userProfile.City;
            PersonalInfo = userProfile.PersonalInfo;
            Photo = userProfile.Photo;
            Gender = userProfile.Gender ?? false;
            Height = userProfile.Height ?? 0;
            Weight = userProfile.Weight ?? 0;
            BlockRange = userProfile.BlockRange ?? 0;
            AttackRange = userProfile.AttackRange ?? 0;
            VolleyballIdol = userProfile.VolleyballIdol;
            PositionId = userProfile.PositionId ?? 0;
        }

    }
}
