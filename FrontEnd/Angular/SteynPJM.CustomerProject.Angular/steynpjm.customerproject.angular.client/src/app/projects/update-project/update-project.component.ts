import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { forkJoin } from 'rxjs';
import { Project } from '../Project';
import { ProjectService } from '../project.service';
import { LookupValue } from '../../LookupValue';

@Component({
  selector: 'app-update-project',
  templateUrl: './update-project.component.html',
  styleUrls: ['./update-project.component.css']
})
export class UpdateProjectComponent implements OnInit {
  project: Project = {
    id: 0,
    deletedIndicator: false,
    version: '',
    name: '',
    code: '',
    description: '',
    managerHid: 0,
    companyHid: 0
  };


  customers: LookupValue[] = [];
  managers: LookupValue[] = [];
  isLoading: boolean = true;

  constructor(
    private projectService: ProjectService,
    private router: Router,
    private route: ActivatedRoute
  ) { }

  ngOnInit(): void {
    const idParam = this.route.snapshot.paramMap.get('id');
    if (idParam) {
      const id = +idParam;
      forkJoin({
        project: this.projectService.getProject(id),
        customers: this.projectService.getCustomerLookup(),
        managers: this.projectService.getManagerLookup()
      }).subscribe(({ project, customers, managers }) => {
          this.project = project;
          this.customers = customers;
          this.managers = managers;
          this.isLoading = false;
        });
    }
  }

  updateProject(): void {
    this.projectService.updateProject(this.project.id, this.project).subscribe(() => {
      this.router.navigate(['/main/projects']);
    });
  }
}
