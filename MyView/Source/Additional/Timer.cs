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
    		m_IsRunning = true;
    		m_Ticks = DateTime.Now.Ticks;
    	}
    	
    	public void Stop()
    	{
    		m_IsRunning = false;
    		m_Ticks = DateTime.Now.Ticks - m_Ticks;
    	}
    	
		/// <summary>
		/// If the timer is stopped this returns the milliseconds elapsed between the last <see cref="Start"/> and <see cref="Stop"/> calls.
		/// If the timer is still running this returns the time between now and last <see cref="Start"/> call.
		/// </summary>
		/// <returns>The elapsed time.</returns>
    	public int GetElapsedTime()
    	{
    		if (m_IsRunning)
    		{
    			return (int)((DateTime.Now.Ticks - m_Ticks) / TimeSpan.TicksPerMillisecond);
    		}
    		else
    		{
    			return (int)(m_Ticks / TimeSpan.TicksPerMillisecond);    			
    		}
    	}
    	#endregion
	}
}
