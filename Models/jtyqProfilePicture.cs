using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using MySqlConnector;

namespace jtyq.Models;

public class UserProfilePicture
{
    public int UserProfilePictureId { get; set; }
    public string? OurTeamId { get; set; }
    public string? ImageName { get; set; }
    public string? ImagePath { get; set; }
    public OurTeam? OurTeam { get; set; }
    
}