export class Settle {
  constructor() { }

  public PayerId: number;
  public SharedMemberId: number;
  public GroupId: number;
  public TotalAmount: number;
}

export class Balance {
  constructor() { }

  public id: number;
  public payer_id: number;
  public receiver_id: number;
  public group_id: number;
  public amount: number;
}
