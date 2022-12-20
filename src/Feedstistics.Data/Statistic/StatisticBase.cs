namespace Feedstistics.Data.Statistic
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Remoting;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class StatisticBase<T> : IStatistic
    {
        /// <summary>
        /// Instansiates an instance of <see cref="StatisticBase{T}"/>.
        /// </summary>
        protected StatisticBase()
        {
        }

        /// <summary>
        /// Instansiates an instance of <see cref="StatisticBase{T}"/> using Entity Framework Statistic entity.
        /// </summary>
        /// <param name="statistic">Entity framework statistic entity.</param>
        public StatisticBase(EF.Models.Entity.Statistics statistic)
        {
            this.Name = statistic.StatisticName;
            this.SampleDateTime = statistic.SampleDateTime;
            this.Dimensions = JsonConvert.DeserializeObject<Dictionary<string, dynamic>?>(statistic.Value) ?? new Dictionary<string, dynamic>();
        }

        /// <summary>
        /// Instansiates an instance of <see cref="StatisticBase{T}"/> given a statistic name and serialized value dimensions.
        /// </summary>
        /// <param name="statisticName">Name of statistic.</param>
        /// <param name="serializedValueDimensions">Serialized value dimensions for this statistic.</param>
        public StatisticBase(string statisticName, string serializedValueDimensions, DateTime sampleDateTimeUtc)
        {
            this.Name = statisticName;
            this.Dimensions = JsonConvert.DeserializeObject<Dictionary<string, dynamic>?>(serializedValueDimensions) ?? new Dictionary<string, dynamic>();
            this.SampleDateTime = sampleDateTimeUtc;
        }

        /// <inheritdoc/>
        public string? Name { get; set; }

        /// <inheritdoc/>
        public DateTime? SampleDateTime { get; set; }

        /// <summary>
        /// Typed Value from within serialized dimensions.
        /// </summary>
        [JsonIgnore]
        public T? Value
        {
            get
            {
                if (this.Dimensions != null && this.Dimensions.Count > 0 && this.Dimensions.TryGetValue("Value", out var value))
                {
                    if (value.GetType().FullName == typeof(T).FullName)
                    {
                        return (T)value;
                    }
                }

                return default;
            }
        }

        /// <inheritdoc/>
        public Dictionary<string, dynamic> Dimensions { get; set; } = new Dictionary<string, dynamic>();
    }
}
