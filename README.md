##Task board management service
Designing and implementing a task board management service to manage task across multiple task boards for users. A single board contains one or more tasks that a assignable to users.

Your task management service should have three main models: Board, Task and User

Some properties of the models:

**Board** ***(Id, Name)***

**Task** ***(Id, Title, Status, CreatedAt, CompletedAt)***

**User** ***(Id, Name, Email)***

###Requirements:
- Service should be able to manage boards, tasks and users
- A task can belong to a single board
- Task title should be unique within a board
- User can be assigned to multiple tasks across multiple boards
- In other to assign a task to a user, the user should first be granted access to that board
  * User can be assigned to tasks within a board
  * Only users with access to a particular board can be assigned to tasks in that board
  * If a user doesn't have access to any board, they cannot be assigned to any task
- Tasks can be in one of the following states: New, InProgress, Completed
- When tasks state changes to "Completed" the assigned user should be cleared
			
Your service should expose REST api for CRUD operations on the respective models (Board, Task, User).

You can choose to use an in-memory datastore or a database (e.g sqlite or any of your choice) to persist your data.
			
###Bonus part (maybe only for seniors?????)
In addition to the requirements above, implement a statistical module to get various statical information by name(stat-name) and expose the module via a single endpoint to get some statistical information by stat-name.

http://<endpoint>/api/statistics?name=<stat-name>

Example of some statistical info we are intersted in include:
|StatName   	| Description  	|
|---	|---	|---	|---	|---	|
| inprogress_task_per_board 	| Count of tasks in "InProgres" state per board |
| inprogress_task_per_user_per_board  	| Count of inprogress tasks for users across boards |
| <more here>  	|   	|


The statistical info provider/module should be flexible enough to extend for future stat requirements without modifying the endpoint

