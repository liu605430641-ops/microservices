namespace Zhaoxi.MSACommerce.SeckillService.Infrastructure;

public static class SecKillDate
{
    /// <summary>
    /// 获取指定日期的凌晨
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public static DateTime ToDayStartHour(this DateTime date)
    {
        var dateString = date.ToString("yyyy-MM-dd");
        dateString += " 00:00:00";
        return DateTime.Parse(dateString);
    }

    /// <summary>
    /// 指定时间往后N个时间间隔
    /// </summary>
    /// <param name="hours"></param>
    /// <returns></returns>
    private static List<DateTime> GetTimePeriods(int hours)
    {
        List<DateTime> timePeriods = [];
        
        // 从凌晨开始循环
        var date = DateTime.Now.ToDayStartHour(); 
        for (var i = 0; i < hours; i++)
        {
            //每次递增2小时,将每次递增的时间存入到List<Date>集合中
            timePeriods.Add(date.AddHours(i * 2));
        }
        return timePeriods;
    }

    /// <summary>
    /// 获取秒杀开始时间列表
    /// </summary>
    /// <returns></returns>
    public static List<DateTime> GetBeginTimes()
    {
        //定义一个List<Date>集合，存储所有时间窗口
        var timePeriods = GetTimePeriods(12);
        //判断当前时间属于哪个时间范围
        var now = DateTime.Now;
        foreach (var timePeriod in timePeriods)
        {
            // 20:00 <= 20:35 < 20:00+2=22:00
            //开始时间<=当前时间<开始时间+2小时
            if (timePeriod.Ticks <= now.Ticks && now.Ticks < timePeriod.AddHours(2).Ticks)
            {
                now = timePeriod;
                break;
            }
        }

        //当前需要显示的时间菜单
        List<DateTime> beginTime = [];
        for (var i = 0; i < 5; i++)
        {
            //20:00,22:00,24:00,02:00,04:00
            beginTime.Add(now.AddHours(i*2));
        }
        return beginTime;
    }

    /// <summary>
    /// 时间转成yyyyMMddHH
    /// </summary>
    /// <param name="date"></param>
    /// <param name="pattern"></param>
    /// <returns></returns>
    public static string ToSecKillTime(this DateTime date)
    {
        return date.ToString("yyyyMMddHH");
    }
}