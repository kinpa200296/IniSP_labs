using System;
using System.Collections.Generic;

namespace ConsolePlayer
{
    public static class LogPageViewManager
    {
        public static int PageSize { get; private set; }

        public static int FirstLogStringOnPageIndex
        {
            get { return CurrentPage * PageSize; }
        }

        public static int FirstLogStringOnNextPageIndex
        {
            get { return Math.Min((CurrentPage + 1) * PageSize, Player.Log.LogSize); }
        }

        public static int CurrentPage { get; private set; }

        public static int PageCount
        {
            get { return Player.Log.LogSize / PageSize + (Player.Log.LogSize % PageSize == 0 ? 0 : 1); }
        }

        public static void Init()
        {
            PageSize = ConsoleDisplayManager.LogFrameHeight - 2;
        }

        public static void NextPage()
        {
            CurrentPage = CurrentPage >= PageCount - 1 ? Math.Max(PageCount - 1, 0) : CurrentPage + 1;
        }

        public static void PreviousPage()
        {
            CurrentPage = CurrentPage == 0 ? 0 : CurrentPage - 1;
        }

        public static void ShowLastPage()
        {
            CurrentPage = PageCount - 1;
        }

        public static string[] GetPageSnapShot()
        {
            var strings = new List<string>();
            if (Player.Log.LogSize == 0)
                return strings.ToArray();
            for (var i = FirstLogStringOnPageIndex; i < FirstLogStringOnNextPageIndex; i++)
            {
                strings.Add(Player.Log.GetByIndex(i));
            }
            return strings.ToArray();
        }
    }
}
