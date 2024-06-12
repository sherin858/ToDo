import { Component, ElementRef, ViewChild } from '@angular/core';
import { Todo } from './to-do.model';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-to-do',
  templateUrl: './to-do.component.html',
  styleUrl: './to-do.component.css',
})
export class ToDoComponent {
  @ViewChild('task') task!: ElementRef<HTMLInputElement>;
  todos: Todo[] = [];
  apiUrl = 'http://localhost:5193/api/ToDo';
  token: string = '';

  constructor(private http: HttpClient) {
    this.token = localStorage.getItem('token') || '';
    this.getTodos();
  }

  getTodos(): any {
    this.http
      .get<Todo[]>(this.apiUrl, {
        headers: new HttpHeaders({
          Authorization: 'Bearer ' + this.token,
        }),
      })
      .subscribe(
        (response) => {
          this.todos = response;
          console.log('Todo retrieved successfully:', response);
        },
        (error) => {
          console.error('Error retrieving todo:', error);
        }
      );
  }

  addTodo(task: string): any {
    this.clearInputs();
    this.http
      .post(
        this.apiUrl,
        {
          title: task,
          isDone: false,
          description: '',
        },
        {
          headers: new HttpHeaders({
            Authorization: 'Bearer ' + this.token,
          }),
        }
      )
      .subscribe(
        (response) => {
          console.log('Todo added successfully:', response);
          this.todos.push({
            ...response,
            title: task,
            isDone: false,
            description: '',
          });
        },
        (error) => {
          console.error('Error adding todo:', error);
        }
      );
  }

  deleteTodo(todoId: string): any {
    const url = `${this.apiUrl}/${todoId!}`;
    this.http
      .delete<void>(url, {
        headers: new HttpHeaders({
          Authorization: 'Bearer ' + this.token,
        }),
      })
      .subscribe(
        (response) => {
          console.log('Todo deleted successfully:', response);
          this.todos = this.todos.filter((item) => item.id !== todoId!);
        },
        (error) => {
          console.error('Error deleting todo:', error);
        }
      );
  }

  updateTodoStatus(todoId: number, completed: boolean): any {
    const url = `${this.apiUrl}/${todoId}`;
    this.http
      .put<Todo>(
        url,
        { completed },
        {
          headers: new HttpHeaders({
            Authorization: 'Bearer ' + this.token,
          }),
        }
      )
      .subscribe(
        (response) => {
          console.log('Todo updated successfully:', response);
        },
        (error) => {
          console.error('Error updating todo:', error);
        }
      );
  }
  clearInputs() {
    this.task.nativeElement.value = '';
  }
}
