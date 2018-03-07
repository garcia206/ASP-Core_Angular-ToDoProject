import { Injectable } from '@angular/core';
import { Component, Inject } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { of } from 'rxjs/Observable/of';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { catchError, map, tap } from 'rxjs/operators';
import { Todo } from '../Models/todo';

// Http options, used multiple times below
const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable()
export class TodoService {

  //URL to Web API
  private apiURL: string;

  constructor( 
    private http: HttpClient, 
    @Inject('BASE_URL') baseUrl: string)   
  {
    this.apiURL = baseUrl + "api/todo";
  }

  // GET /api/todo
  getAll(): Observable<Todo[]> {
    return this.http.get<Todo[]>(this.apiURL)
    .pipe(
      tap(td => console.log("Fetched all TODO items"))
    );
  }

  // POST /api/todo
  add(tdItem: Todo): Observable<Todo> {
    return this.http.post<Todo>(this.apiURL, tdItem, httpOptions)
    .pipe(
      tap((td: Todo) => console.log(`Added new ToDo Item w/ id=${td.id}`)))
  }

  // DELETE /api/todo/{id}
  remove(tdItem: Todo | number): Observable<Todo> {
    const id = typeof tdItem === 'number' ? tdItem : tdItem.id;
    const url = `${this.apiURL}/${id}`;

    return this.http.delete<Todo>(url, httpOptions).pipe(
      tap(_ => console.log(`Deleting hero id=${id}`))
    )
  }

  // PUT /api/todo/{id}
  update(tdItem: Todo) {
    const id = tdItem.id;
    const url = `${this.apiURL}/${id}`;

    return this.http.put(url, tdItem, httpOptions).pipe(
      tap(_ => console.log(`Updated Todo status id=${tdItem.id}`))
    );
  }
}
