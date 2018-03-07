import { Component, OnInit } from '@angular/core';
import { TodoService } from '../Services/todo.service';
import { Todo } from '../Models/todo';


@Component({
  selector: 'app-todo',
  templateUrl: './todo.component.html',
  styleUrls: ['./todo.component.css']
})
export class TodoComponent implements OnInit {
  todoList: Todo[];

  constructor(private todoService: TodoService) { }

  ngOnInit() {
    this.getTodoList();
  }

  // GET All ToDo Items
  getTodoList(): void {
    this.todoService.getAll().subscribe(td => this.todoList = td);
  }

  //POST New ToDo Item
  addItem(name: string): void {
    // id and isComplete fields are automatically generated, so I don't capture them
    name = name.trim();

    if(!name) {return};
    this.todoService.add({name} as Todo).subscribe( td => {
      this.todoList.push(td)
    });
  }

  // DELETE ToDo Item
  deleteItem(tdItem : Todo): void{
    this.todoService.remove(tdItem).subscribe(_ => {
      this.todoList = this.todoList.filter(td => td !== tdItem)
    });
  }

  // PUT Update ToDo Item
  updateItem(tdItem: Todo): void {

    // This is incorrect, I'm changing the status in UI before confirming server change
    tdItem.isComplete = !tdItem.isComplete;
    this.todoService.update(tdItem).subscribe();

  }

}
