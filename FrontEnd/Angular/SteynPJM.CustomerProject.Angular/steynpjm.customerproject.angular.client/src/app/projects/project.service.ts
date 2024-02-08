import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Project } from './Project';
import { LookupValue } from "../LookupValue";

@Injectable({
  providedIn: 'root'
})
export class ProjectService {
  private apiUrl = '/project';

  httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('token')
    })
  };

  constructor(private http: HttpClient) { }

  getProjects(): Observable<Project[]> {
    return this.http.get<Project[]>(`${this.apiUrl}/list`, this.httpOptions);
  }

  getProject(id: number): Observable<Project> {
    return this.http.get<Project>(`${this.apiUrl}/${id}`, this.httpOptions);
  }

  addProject(Project: Project): Observable<Project> {
    return this.http.post<Project>(this.apiUrl, Project, this.httpOptions);
  }

  updateProject(id: number, Project: Project): Observable<Project> {
    return this.http.put<Project>(`${this.apiUrl}/${id}`, Project, this.httpOptions);
  }

  deleteProject(id: number): Observable<Project> {
    return this.http.delete<Project>(`${this.apiUrl}/${id}`, this.httpOptions); arguments
  }

  getCustomerLookup(): Observable<LookupValue[]> {
    return this.http.get<LookupValue[]>(`/company/lookup`, this.httpOptions);
  }

  getManagerLookup(): Observable<LookupValue[]> {
    return this.http.get<LookupValue[]>(`/user/lookup`, this.httpOptions);
  }
}
