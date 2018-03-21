using Microsoft.Bot.Builder.FormFlow;
using pc.BudapestHotel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace pc.BudapestHotel.Forms
{
    [Serializable]
    class RoomReservationForm : IRoomReservation
    {
        bool finalConfirmation = false;
        #region IRoomReservation 
        public string FullName { get; set; }
        public DateTime? CheckInDate { get; set; }
        public DateTime? CheckOutDate { get; set; }
        public RoomType RoomType { get; set; }
        public int Occupency { get; set; }

        public List<Extras> Extras { get; set; }
        public bool VIPMembership { get; set; }
        #endregion

        #region Constructors
        public RoomReservationForm(params object[] values)
        {

            foreach (var v in values)
            {
                int number;
                if (v is IDateRange)
                {
                    var dateRange = v as IDateRange;
                    this.CheckInDate = dateRange.StartDateTime;
                    this.CheckOutDate = dateRange.EndDateTime;
                }
                else if (int.TryParse(v.ToString(), out number))
                {
                    this.Occupency = number;
                }
            }
        }
        public RoomReservationForm(IDateRange dateRange)
        {
            this.CheckInDate = dateRange.StartDateTime;
            this.CheckOutDate = dateRange.EndDateTime;
        }

        public RoomReservationForm(IDateRange dateRange, int occupency) : this(dateRange)
        {
            this.Occupency = occupency;
        }
        #endregion

        public static IForm<RoomReservationForm> BuildForm()
            => new FormBuilder<RoomReservationForm>()
            .Message("Let me help you book a room")
            .Field(nameof(FullName))
            .Field(nameof(CheckInDate), state => !state.CheckInDate.HasValue || state.finalConfirmation)
            .Field(nameof(CheckOutDate), state => !state.CheckOutDate.HasValue)
            .Field(nameof(Occupency), state => state.Occupency == 0)
            .AddRemainingFields()
            .Confirm(
                prompt: "are the following valid?{*}{||}",
              condition: state => state.finalConfirmation = true,
                dependencies: typeof(IRoomReservation).GetProperties().Select(p => p.Name))

            .Build();

    }
}