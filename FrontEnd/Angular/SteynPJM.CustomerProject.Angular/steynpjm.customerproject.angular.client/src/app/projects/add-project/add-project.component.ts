import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { NgForm, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { forkJoin } from 'rxjs';
import { Project } from '../Project';
import { ProjectService } from '../project.service';
import { LookupValue } from '../../LookupValue';

@Component({
  selector: 'app-add-project',
  templateUrl: './add-project.component.html',
  styleUrls: ['./add-project.component.css']
})
export class AddProjectComponent implements OnInit, AfterViewInit {

  @ViewChild('projectForm') projectForm!: NgForm;

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


  constructor(private projectService: ProjectService, private router: Router) { }

  ngOnInit(): void {
    forkJoin({
      customers: this.projectService.getCustomerLookup(),
      managers: this.projectService.getManagerLookup()
    }).subscribe(({ customers, managers }) => {
      this.customers = customers;
      this.managers = managers;
      this.isLoading = false;
    });
  }

  ngAfterViewInit(): void {
  }


  onSubmit(): void {
    if (!this.projectForm.valid) {
      return;
    }
    this.projectService.addProject(this.project).subscribe(() => {
      this.router.navigate(['main/projects']);
    });
  }

}
