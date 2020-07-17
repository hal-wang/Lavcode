using SQLite;
using System;
using System.Text;

namespace Hubery.Lavcode.Uwp.Helpers.Logger
{
    public class Log
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public string Version { get; set; }

        public DateTime Time { get; set; }

        public string OperatingSystemVersion { get; set; }

        public string DeviceModel { get; set; }

        public string AppName { get; set; }

        public string Platform { get; set; }


        public string Source { get; set; }

        public string Content { get; set; }

        public string ToString(bool showDetail = true)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("时间：");
            sb.Append(Time.ToString());
            sb.AppendLine();

            sb.Append("软件版本：");
            sb.Append(Version);
            sb.AppendLine();

            sb.Append("操作系统版本：");
            sb.Append(OperatingSystemVersion);
            sb.AppendLine();

            if (showDetail)
            {
                sb.Append("机器型号：");
                sb.Append(DeviceModel);
                sb.AppendLine();

                sb.Append("APP：");
                sb.Append(AppName);
                sb.AppendLine();

                sb.Append("平台：");
                sb.Append(Platform);
                sb.AppendLine();
            }

            sb.Append("来源：");
            sb.Append(Source);
            sb.AppendLine();

            sb.Append("内容：");
            sb.Append(Content);

            return sb.ToString();
        }
    }
}