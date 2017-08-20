using System;

namespace MyView.Additional
{
	/// <summary>
	/// A simple timer that tracks elapsed time.
	/// </summary>
	class Timer
	{
		#region VARIABLES
		private bool m_IsRunning;
    	private long m_Ticks;
    	#endregion
    	
    	
		#region PUBLIC API
    	public void Start()
    	{
    		if (!m_IsRunning)
    		{
    			m_IsRunning = true;
    			m_Ticks = DateTime.Now.Ticks;
    		}
    		else
			{
				Console.WriteLine("Cannot start timer as it is already running.");
    		}
    	}
    	
    	public void Stop()
    	{
    		if (m_IsRunning)
    		{
    			m_IsRunning = false;
    			m_Ticks = DateTime.Now.Ticks - m_Ticks;
    		}
    		else
			{
				Console.WriteLine("Cannot stop timer as it is not running.");
    		}
    	}
    	
		/// <summary>
		/// Returns the milliseconds elapsed between the last <see cref="Start"/> and <see cref="Stop"/> calls.
		/// </summary>
		/// <returns>The elapsed time.</returns>
    	public int GetElapsedTime()
    	{
    		return (int)(m_Ticks / TimeSpan.TicksPerMillisecond);
    	}
    	#endregion
	}
}
