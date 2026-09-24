export interface Customer{
  id: string
  name: string
  firstName: string
  address: string
  email: string
  isActive: boolean
}

export interface Order{
  id: number
  amount: number
  customerId: number
  createdAt: number

}

export interface IResult<T>{
  status: ResultState,
  data: T,
  errorMessage?: string
}

export enum ResultState{
  pending = "pending",
  success = "success",
  failed = "failed"
}
