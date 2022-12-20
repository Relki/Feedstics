namespace Feedstistics.Data
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Statistic.
    /// </summary>
    public interface IStatistic
    {
        /// <summary>
        /// Name of statistic.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Sample date time of when statistic was collected.
        /// </summary>
        public DateTime? SampleDateTime { get; set; }

        /// <summary>
        /// Dimensions data containing all relevant fields including Value.
        /// </summary>
        public Dictionary<string, dynamic> Dimensions { get; set; }
    }
}
