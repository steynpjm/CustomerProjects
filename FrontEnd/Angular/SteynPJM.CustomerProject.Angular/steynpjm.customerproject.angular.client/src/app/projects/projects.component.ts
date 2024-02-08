import { Component, OnInit } from '@angular/core';
import { Project } from './Project';
import { ProjectService } from './project.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-projects',
  templateUrl: './projects.component.html',
  styleUrls: ['./projects.component.css']
})
export class ProjectsComponent implements OnInit {
  projects: Project[] = [];

  constructor(private projectService: ProjectService, private router: Router) { }

  ngOnInit(): void {
    this.getProjects();
  }

  getProjects(): void {
    this.projectService.getProjects().subscribe(projects => this.projects = projects);
  }

  addProject(): void {
    // Navigate to the AddProjectComponent
    this.router.navigate(['main/projects/add']);
  }

  updateProject(project: Project): void {
    // Navigate to the UpdateProjectComponent with the user data
    this.router.navigate(['main/projects/update', project.id]);
  }

  deleteProject(project: Project): void {
    this.projectService.deleteProject(project.id).subscribe(() => {
      this.projects = this.projects.filter(u => u !== project);
    });
  }
}
