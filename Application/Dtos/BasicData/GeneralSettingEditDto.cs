using System;

namespace Application.Dtos.BasicData;
public class GeneralSettingEditDto
{
    public int? Id { get; set; }
    public int NumberRefreshDoctorMinute { get; set; }
    public int NumberResendRequestMinute { get; set; }
    public int NumberHoursStudyNormalRunnigOut { get; set; }
    public int NumberHoursStudyEmergencyRunnigOut { get; set; }
    public int NumberHoursStudyOutTime { get; set; }
    public string URLServerAutoUpload { get; set; }
    public float NumberMinuteServerAutoUpload { get; set; }
}

