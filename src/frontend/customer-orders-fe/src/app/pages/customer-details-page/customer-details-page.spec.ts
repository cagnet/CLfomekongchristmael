import { ComponentFixture, TestBed } from '@angular/core/testing';
import { CustomerDetailsPage } from './customer-details-page';

describe('CustomerDetailsPage', () => {
  let component: CustomerDetailsPage;
  let fixture: ComponentFixture<CustomerDetailsPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CustomerDetailsPage],
    }).compileComponents();

    fixture = TestBed.createComponent(CustomerDetailsPage);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
