using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SampleLib.Threading {
	public static class TaskExtensions {
		// More Effective　C# 6.0/7.0 p.133
		/// <summary>
		/// 
		/// </summary>
		/// <param name="task"></param>
		/// <param name="onError"></param>
		/// <returns></returns>
		public static async Task FireAndForget(this Task task, Action<Exception> onError) {
			try {
				await task;
			} catch (Exception exception) {
				onError(exception);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="task"></param>
		/// <param name="onError"></param>
		/// <returns></returns>
		public static async Task FireAndForget(this Task task, Func<Exception, bool> onError) {
			try {
				await task;
			} catch (Exception exception) when (onError(exception)) {
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TException"></typeparam>
		/// <param name="task"></param>
		/// <param name="recovery"></param>
		/// <param name="onError"></param>
		/// <returns></returns>
		public static async Task FireAndForget<TException>(
			this Task task,
			Action<TException> recovery,
			Func<Exception, bool> onError) where TException : Exception {
			try {
				await task;
				// ロギングを行うonErrorメソッドが常にfalseを返すことに依存する
			} catch (Exception exception) when (onError(exception)) {
			} catch (TException exception) {
				recovery(exception);
			}
		}
	}
}
