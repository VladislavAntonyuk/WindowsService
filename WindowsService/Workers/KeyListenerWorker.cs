using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using H.Hooks;
using Microsoft.Extensions.Hosting;
using WindowsService.Services.LogService;

namespace WindowsService.Workers;

public class KeyListenerWorker : BackgroundService
{
	private LowLevelKeyboardHook? _hookEvents;

	protected override Task ExecuteAsync(CancellationToken stoppingToken)
	{
		_hookEvents = new LowLevelKeyboardHook();
		_hookEvents.Down += OnKeyDown;
		_hookEvents.Start();

		LogService.Log("Global key listener started. Listening for Ctrl+Alt+J...");

		stoppingToken.WaitHandle.WaitOne();
		return Task.CompletedTask;
	}

	private void OnKeyDown(object? sender, KeyboardEventArgs e)
	{
		if (e is { CurrentKey: Key.J, Keys: { IsCtrl: true, IsAlt: true } })
		{
			LogService.Log("Ctrl+Alt+J detected!");
			Process.Start(@"C:\Windows\system32\scrnsave.scr", "/s");
			//PowerBoard.TurnOffLCD();
		}
	}

	public override void Dispose()
	{
		if (_hookEvents != null)
		{
			_hookEvents.Down -= OnKeyDown;
			_hookEvents.Stop();
			_hookEvents.Dispose();
		}

		base.Dispose();
	}
}