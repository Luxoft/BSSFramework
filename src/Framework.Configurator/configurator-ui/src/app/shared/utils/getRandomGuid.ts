export function getRandomGuid(): string {
  function gen(): string {
    return Math.floor((1 + Math.random()) * 0x10000)
      .toString(16)
      .substring(1);
  }

  return `${gen()}${gen()}-${gen()}-${gen()}-${gen()}-${gen()}${gen()}${gen()}`;
}
