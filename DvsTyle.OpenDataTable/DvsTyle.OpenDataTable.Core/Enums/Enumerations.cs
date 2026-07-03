using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace DvStyle.OpenDataTable.Enums
{
    public enum ErrorMessageType
    {
        Custom = 1,
        System = 2
    }
    public class CustomJsonResponseStatus
    {
        public const int OK_DISPLAY_CONFIRMATION = 1;
        public const int KO_DISPLAY_MODELSTATE_ERRORS = 2;
        public const int KO_DISPLAY_FORM = 3;
        public const int KO_DISPLAY_FORM_MODAL = 4;
        public const int OK_CLOSE_MODAL_REFRESH_GRID = 5;
        public const int OK_CLOSE_MODAL = 6;
        public const int OK_REFRESH_WINDOW_FROM_MODAL = 7;
        public const int OK_DISPLAY_ALERT_CONFIRMATION = 8;
        public const int AJAX_SUBMIT_RELOAD = 9;
        public const int GET_SELECTOR_DATAS = 10;
        public const int OK_REFRESH_WINDOW_FROM_BUTTON = 11;
        public const int REDIRECT_TO_LOCATION = 12;
        public const int REFRESH_DATATABLE = 13;
        public const int KO_DISPLAY_ALERT_MESSAGE = 14;
        public const int DISPLAY_PAGE_CONTENT = 15;
        public const int CREATE_MODAL = 16;
        public const int REFRESH_NOTIFICATION = 17;
        public const int CLOSE_CURENT_MODAL = 18;
        public const int CREATE_DYNAMIC_MODAL = 19;
        public const int OK_CLOSE_MODAL_REFRESH_CALENDAR = 20;
        public const int DISPLAY_TRACKING_RESULT = 21;
        public const int CLOSE_MODAL_DISPLAY_PAGE_CONTENT = 22;
        public const int REFETCH_IMG = 23;
        public const int DISPLAY_PAGE_PARTIAL_VIEW = 24;
    }

    public enum IhmComponent
    {
        Form = 1,
        DataTable = 2,
        Calendar = 3
    }

    public enum SwitchView
    {
        List = 1,
        Grid = 2
    }

    public enum DataSelectorType
    {
        AddValue = 1,
        SearchValue = 2
    }

    public enum JQueryDataTableSearchInputType
    {
        Text = 1,
        EnumList = 2,
        Calendar = 3,
        Number = 4,
        BooleanList = 5,
        TimeCalendar = 6,
        InputFileUpload = 7
    }
}