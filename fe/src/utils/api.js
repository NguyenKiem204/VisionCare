export const mockDelay = (ms = 500) =>
  new Promise((res) => setTimeout(res, ms));

export const api = {
  async createBooking(payload) {
    await mockDelay(700);
    return { ok: true, code: "VC-2024-0001", payload };
  },
};
