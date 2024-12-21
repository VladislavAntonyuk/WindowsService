using System.Runtime.InteropServices;

namespace WindowsService.Services;

internal class PowerBoard
{
	[DllImport("user32.dll")]
	private static extern int SendMessage(int hWnd, int hMsg, int wParam, int lParam);

	[DllImport("user32.dll", SetLastError = true)]
	private static extern nint FindWindow(string? lpClassName, string? lpWindowName);

	public static void TurnOffLCD()
	{
		SendMessage(FindWindow(null, null).ToInt32(), WM_SYSCOMMAND, SC_MONITORPOWER, MONITOR_OFF);
	}

	private const int MONITOR_OFF = 2;
	private const int WM_SYSCOMMAND = 274;
	private const int SC_MONITORPOWER = 61808;
}