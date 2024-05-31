import random
import sys
import requests

BASE_URL = "http://localhost:5113/api"

# Bulk inserts from random data APIs


def insert_random_users(count):
    url = f"https://randomuser.me/api/?results={count}"
    response = requests.get(url)
    data = response.json()
    names = [user["name"]["first"] for user in data["results"]]
    user_ids = []

    for name in names:
        try:
            user_id = insert_user(name)
            user_ids.append(user_id)
        except:
            print(f"Failed to insert user {name}")

    return user_ids


def insert_random_files(count):
    url = f"https://random-word-api.herokuapp.com/word?number={count}"
    response = requests.get(url)
    words = response.json()
    file_ids = []

    for word in words:
        file_name = f"{word}.txt"
        try:
            file_id = insert_file(file_name, risk=random.randint(1, 100))
            file_ids.append(file_id)
        except:
            print(f"Failed to insert file {file_name}")

    return file_ids


def insert_random_groups(count):
    url = f"https://random-word-api.herokuapp.com/word?number={count}"
    response = requests.get(url)
    words = response.json()
    group_ids = []

    for word in words:
        group_name = f"Group {word}"
        try:
            group_id = insert_group(group_name)
            group_ids.append(group_id)
        except:
            print(f"Failed to insert group {group_name}")

    return group_ids


# Server calls methods


def insert_user(name):
    url = f"{BASE_URL}/users"
    response = requests.post(url, json={"name": name})
    id = response.json()
    print(f"Inserted user '{id}' with name '{name}'")
    return id


def insert_file(name, risk):
    url = f"{BASE_URL}/files"
    response = requests.post(url, json={"name": name, "risk": risk})
    id = response.json()
    print(f"Inserted file '{id}' with name '{name}' and risk {risk}")
    return id


def insert_group(name):
    url = f"{BASE_URL}/groups"
    response = requests.post(url, json={"name": name})
    id = response.json()
    print(f"Inserted group '{id} with name '{name}'")
    return id


def share_files(file_ids, user_ids, group_ids):
    for file_id in file_ids:
        random_user_ids = random.sample(user_ids, random.randint(0, len(user_ids)))
        random_group_ids = random.sample(group_ids, random.randint(0, len(group_ids)))
        payload = {"users": random_user_ids, "groups": random_group_ids}
        requests.post(f"{BASE_URL}/files/{file_id}/share", json=payload)
        print(f"Shared file {file_id} with users {random_user_ids} and groups {random_group_ids}")


def add_users_to_groups(group_ids, user_ids):
    for group_id in group_ids:
        random_user_ids = random.sample(user_ids, random.randint(0, len(user_ids)))
        requests.post(f"{BASE_URL}/groups/{group_id}/users", json=random_user_ids)
        print(f"Added users {random_user_ids} to group {group_id}")


def main():
    num_files = int(sys.argv[2]) if len(sys.argv) > 1 else 5
    num_users = int(sys.argv[1]) if len(sys.argv) > 2 else 5
    num_groups = int(sys.argv[3]) if len(sys.argv) > 3 else 5

    file_ids = insert_random_files(num_files)
    user_ids = insert_random_users(num_users)
    group_ids = insert_random_groups(num_groups)

    share_files(file_ids, user_ids, group_ids)
    add_users_to_groups(group_ids, user_ids)


if __name__ == "__main__":
    main()
