import { PostSelectedTopIssueAsync} from '@/api/reqCategory.js'
import { ref } from 'vue'

const newSelected = ref({
    Uid_Bot_Category: '',
})

const errorSelected = ref({
  name: { isError: false, message: '' },
})


async function HandlePublishSelected() {

  const postSelected = await PostSelectedTopIssueAsync(
    newSelected.value.Uid_Bot_Category
  )

  if (!postSelected.is_success) {
    if (postSelected.error.statuscode === 400) {
      errorSelected.value.name.isError = true
      errorSelected.value.name.message = postSelected.error.message
    }
    return postSelected.error.statuscode
  }
  return true
}



export { newSelected, errorSelected, HandlePublishSelected}