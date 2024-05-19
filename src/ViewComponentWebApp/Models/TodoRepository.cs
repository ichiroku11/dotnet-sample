namespace ViewComponentWebApp.Models;

public class TodoRepository {
	private static readonly IList<Todo> _todos = [
		new Todo("Task 1", true),
		new Todo("Task 2", true),
		new Todo("Task 3", true),
		new Todo("Task 4", false),
		new Todo("Task 5", false),
		new Todo("Task 6", false),
	];

	public Task<IEnumerable<Todo>> GetTodosAsync(bool isDone) {
		var todos = _todos.Where(todo => todo.IsDone == isDone);
		return Task.FromResult(todos);
	}
}
