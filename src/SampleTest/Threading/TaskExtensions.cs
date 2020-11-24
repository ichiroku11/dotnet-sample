using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SampleTest.Threading {
	public static class TaskExtensions {
		// More Effective　C# 6.0/7.0 p.133
		public static async Task FireAndForget(this Task task, Action<Exception> onError) {
			try {
				await task;
			} catch (Exception exception) {
				onError(exception);
			}
		}

		public static async Task FireAndForget(this Task task, Func<Exception, bool> onError) {
			try {
				await task;
			} catch (Exception exception) when (onError(exception)) {
			}
		}

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
